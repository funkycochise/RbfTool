using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace RBfTool
{
    class RbfManager
    {

        //
        String inputÌnjectedRbfFileName = "";
        String outputÌnjectedRbfFileName = "";
        //
        String inputRbfFileName = "";
        String outputRbfFileName = "";
        //
        String inputRomFileName = "";
        String outputRomFileName = "";

        byte[] header = new byte[16];
        int headerSz = 16;
        byte[] headerMister = new byte[6];
        int headerMisterSz = 6;
        byte[] headerRbfSize = new byte[4];
        int headerRbfSz = 4;

        byte[] injectedRfContent;
        byte[] rbfContent;
        byte[] romContent;

        public String getRomLength() {
            int i;
            i = romContent.Length;
            return i.ToString("X4");
        }

        public String getRbfLength()
        {
            int i;
            i = rbfContent.Length;
            return i.ToString("X4");
        }

        public String getinjectedRbfLength()
        {
            int i;
            i = injectedRfContent.Length;
            return i.ToString("X4");
        }

        public bool LoadRbf(String filename) {
            bool retVal=false;

            // suppose that it's an injected rbf
            inputÌnjectedRbfFileName = filename;
            injectedRfContent = File.ReadAllBytes(inputÌnjectedRbfFileName);

            for (int i = 0; i < headerSz; i++)
            {
                header[i] = injectedRfContent[i];
            }
            for (int i = 0; i < headerMisterSz; i++)
            {
                headerMister[i] = injectedRfContent[i];
            }
            string result = System.Text.Encoding.UTF8.GetString(headerMister);
            if (result == "MiSTer")
            {
                for (int i = 0; i < (headerRbfSz); i++)
                {
                    headerRbfSize[i] = injectedRfContent[i + 12];
                }
                retVal = true;

                // Adress where rom starts
                int romOffset = BitConverter.ToInt32(headerRbfSize, 0) + 16;

                // get the Rbf content
                int j = 0;
                int rbfsize = romOffset - 16;
                rbfContent = new byte[rbfsize];
                for (int i = 16; i < romOffset; i++)
                {
                    rbfContent[j++] = injectedRfContent[i];
                }

                // get the rom content
                j = 0;
                int romsize = injectedRfContent.Length - romOffset;
                romContent = new byte[romsize];
                for (int i = romOffset; i < injectedRfContent.Length; i++)
                {
                    romContent[j++] = injectedRfContent[i];
                }
                
            }else{
                // supposed normal rbf
                injectedRfContent = null;
                inputÌnjectedRbfFileName = "";
                inputRbfFileName = filename;
                rbfContent = File.ReadAllBytes(inputRbfFileName); 
            }

            return retVal;
        }

        public void PrepareInjectedRbf(){
            // compute size
            // header 16 octets
            // rbf
            // rom
            int size = header.Length + rbfContent.Length + romContent.Length;
            injectedRfContent = new byte[size];

            //prepare MiSTer header
            headerMister = Encoding.ASCII.GetBytes("MiSTer");
            //prepare rbf size header
            String s = rbfContent.Length.ToString("X4");
            headerRbfSize = new byte[4];
            byte[] bytes = BitConverter.GetBytes(Convert.ToInt32(s, 16));
            int id=0;
            foreach(byte b in bytes) {
                headerRbfSize[id++] = b;
            }
            //build injected content
            int index = 0;
            // mister header
            for (int i=0;i<headerMister.Length;i++) {
                injectedRfContent[index++] = headerMister[i];
            }
            // 6 blank bytes
            for (int i=0;i<6;i++) {
                injectedRfContent[index++] = 0;
            }
            // size header
            for (int i = 0; i < headerRbfSize.Length; i++)
            {
                injectedRfContent[index++] = headerRbfSize[i];
            }
            //rbf
            for (int i = 0; i < rbfContent.Length; i++)
            {
                injectedRfContent[index++] = rbfContent[i];
            }
            //rom
            for (int i = 0; i < romContent.Length; i++)
            {
                injectedRfContent[index++] = romContent[i];
            }
            //save test
            //File.WriteAllBytes("E:\\injtest.rbf", injectedRfContent);
        }

        public void SaveRbf(String filename)
        {
            outputRbfFileName = filename;
            File.WriteAllBytes(outputRbfFileName, rbfContent);
        }

        public void SaveInjectedRbf(String filename) {
            outputÌnjectedRbfFileName = filename;
            File.WriteAllBytes(outputÌnjectedRbfFileName, injectedRfContent);
        }

        public void SaveRom(String filename) {
            outputRomFileName = filename;
            File.WriteAllBytes(outputRomFileName, romContent);
        }

        public bool LoadRom(String filename){
            bool retVal;
            inputRomFileName = filename;
            romContent = File.ReadAllBytes(inputRomFileName);

            PrepareInjectedRbf();
            retVal = true;
            return retVal;
        }

        public bool AutoInject(String folder){
            bool retVal=false;

            ArrayList rbfList = new ArrayList();
            ArrayList romList = new ArrayList();
            ArrayList rbfinjList = new ArrayList();

            //list content of folder
            string[] content = Directory.GetFiles(folder);

            foreach(string s in content) {
                Console.WriteLine(s);
            }
   

            return retVal;
        }
    }
}
