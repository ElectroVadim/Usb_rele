using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Interfaces;
using LibUsbDotNet;
using LibUsbDotNet.Main;
using Models;

namespace Keyboard
{
    public class Grifon: IKeyboard
    {
        private IUsbDevice _device;
        private int _light;
        private bool _enable;
        private int _lastKey;
        private int _key;
        private int _enableKey;
        private bool _emulation;
        private DateTime _sleep;
        //private int _gameCount;
        private long _deviceId;
        private int _lastSetKeyboardValue = -1;
        private UInt32 reley = 0;
        private bool _verification = false;
        

        private void MainThread()
        {
            while(true)
            {
                if(!_enable)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    if(_device == null)
                    {
                        var finder = new UsbDeviceFinder(0x10C4, 0x0005);

                        Status = DeviceStatus.Disconnect;

                        _device = (IUsbDevice)UsbDevice.OpenUsbDevice(finder);


                        if (_device == null)
                        {
                            Status = DeviceStatus.Error;
                            
                            //_enable = false;
                            continue;
                        }

                        Status = DeviceStatus.Active;
                        Enable();
                    }

                    byte[] data = new byte[4];

                    data[0] = (byte)(reley >> 24); 
                    data[1] = (byte)(reley >> 16);
                    data[2] = (byte)(reley >> 8);
                    data[3] = (byte)reley;


                    int count;

                   // new byte[]{0xff, 0xff, 0xff, 0xff}

                    using(var write = _device.OpenEndpointWriter(WriteEndpointID.Ep02, EndpointType.Bulk))
                    {
                        write.Write(data, 50, out count);
                    }

                    if (count == 0)
                    {
                        Status = DeviceStatus.Error;
                        //_enable = false;
                        _device.Close();
                        _device = null;
                        continue;
                        //throw new Exception("Error write port (Grifon)");
                    }

                    var buffer = new byte[64];

                    using (var read = _device.OpenEndpointReader(ReadEndpointID.Ep01, 64, EndpointType.Bulk))
                    {
                        read.Read(buffer, 50, out count);
                    }

                    if (count == 0)
                    {
                        Status = DeviceStatus.Error;
                        _device.Close();
                        _device = null;
                        //_enable = false;
                        continue;
                        Console.WriteLine("[ERROR]" + "Read from port error");
                        //throw new Exception("Error read port (Grifon)");
                    }


                    if ( count == 2 )
                    {
                        string tmp = "";

                        tmp += (char) buffer[0];
                        tmp += (char) buffer[1];

                        if (tmp == "ok")
                        {
                            _verification = true;
                        }
                        else
                        {
                            _verification = false;
                        }

                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString(), true);
                    Status = DeviceStatus.Error;
                    _enable = false;
                    Thread.Sleep(150);
                    continue;
                }

                Thread.Sleep(100);
            }
        }


        public bool verification()
        {
            return _verification;
        }

        public DeviceStatus Status { get; set; }
        public string Port { get; set; }
        public long DeviceId { get { return _deviceId; } }
        public int Baud { get; set; }

        public event Action<int> PressedKey;

        public Grifon()
        {
            Status = DeviceStatus.Active;
            new Thread(MainThread) { IsBackground = true }.Start();

            _sleep = DateTime.Now;
        }
        public bool Init()
        {
            if(_device != null)
            {
                _enable = false; Thread.Sleep(1000);

                _device.Close();
                _device = null;
            }



            var finder = new UsbDeviceFinder(0x10C4, 0x0005);



            Status = DeviceStatus.Disconnect;

            _device = (IUsbDevice)UsbDevice.OpenUsbDevice(finder);

            if(_device == null) return false;

            //Console.WriteLine("Keyboard is connected");
            Status = DeviceStatus.Active;

            _device.SetConfiguration(1);
            _device.ClaimInterface(0);
            using(var write = _device.OpenEndpointWriter(WriteEndpointID.Ep02, EndpointType.Bulk))
            {
                int t;
                write.Write(new byte[] { 0, 0 }, 50, out t);
                t++;
            }


            _enable = true;

            #region KostIL
            if(_lastSetKeyboardValue > 0)
                SetLight(_lastSetKeyboardValue);
            #endregion KostIL

            return true;
        }
        public bool Reset()
        {
            try
            {
                return Init();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return Init();
            }
        }

        public void SetLight(int value)
        {

        }
        public void SetRele(UInt32 value)
        {
            reley = value;
        }
        public void SetRele(int number,bool value)
        {
            if (value)
            {
                reley |= (UInt32) (1 << number);
            }
            else
            {
                reley &= ~(UInt32)(1 << number);
            }
        }

        public void SetLight(String s)
        {

        }
        public void Dispose()
        {

        }

        public void Enable()
        {
            _enable = true;
        }

        public bool Enabled()
        {
            return _enable;
        }

        public void Disable()
        {
            _enable = false;
        }
    }
}
