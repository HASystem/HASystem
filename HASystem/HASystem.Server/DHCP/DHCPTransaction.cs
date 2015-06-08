using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASystem.Server.DHCP
{
    public class DHCPTransaction
    {
        public DHCPMessage Message;
        public DHCPData Data;
        public const int OPTION_OFFSET = 240;

        #region ctor
        public DHCPTransaction(byte[] data)
        {
            BinaryReader reader;
            MemoryStream memoryStream = new MemoryStream(data, 0, data.Length);

            try
            {
                //initialize the binary reader
                reader = new BinaryReader(memoryStream);
                //read data
                Message.D_op = reader.ReadByte();
                Message.D_htype = reader.ReadByte();
                Message.D_hlen = reader.ReadByte();
                Message.D_hops = reader.ReadByte();
                Message.D_xid = reader.ReadBytes(4);
                Message.D_secs = reader.ReadBytes(2);
                Message.D_flags = reader.ReadBytes(2);
                Message.D_ciaddr = reader.ReadBytes(4);
                Message.D_yiaddr = reader.ReadBytes(4);
                Message.D_siaddr = reader.ReadBytes(4);
                Message.D_giaddr = reader.ReadBytes(4);
                Message.D_chaddr = reader.ReadBytes(16);
                Message.D_sname = reader.ReadBytes(64);
                Message.D_file = reader.ReadBytes(128);
                Message.M_Cookie = reader.ReadBytes(4);
                Message.D_options = reader.ReadBytes(data.Length - OPTION_OFFSET);
            }
            catch
            {
                // AppendError(ex.Message, ClassName, "cDHCPStruct(byte[] Data)");
            }
            finally
            {
                if (memoryStream != null) { memoryStream.Dispose(); }

                memoryStream = null;
                reader = null;
            }
        }
        #endregion

        
      
    }
}
