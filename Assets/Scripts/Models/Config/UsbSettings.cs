using System;

namespace Assets.Scripts.Models.Config
{
    public record UsbSettings
    {
        public int VendorId;
        public int ProductId;
        public int Interface;
        public byte Endpoint;
        public string TransferType;
        public int PacketSize;
        public int ReadTimeout;
        public int ReadInterval;
        public int CameraStartOrientation;
        public float CameraStartDistance;

        internal static UsbSettings GetDefault()
        {
            return new UsbSettings
            {
                VendorId = 0x7CBA,
                ProductId = 0x0ABC,
                Interface = 0,
                Endpoint = 0x81,
                TransferType = "Bulk",
                PacketSize = 512,
                ReadTimeout = 500,
                ReadInterval = 100,
                CameraStartOrientation = 1,
                CameraStartDistance = 150f
            };
        }
    }
}
