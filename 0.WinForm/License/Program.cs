using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ESS.Framework.Licensing;
using ESS.Framework.Licensing.License;

namespace License
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Choice<1(Generate)/2(Test)/9(Key)>:");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    GenerateLicense(); break;
                case "2":
                    TestLicense(); break;
                case "9":
                    GenerateKey(); break;
            }


        }

        /// <summary>
        /// 生成密匙
        /// </summary>
        private static void GenerateKey()
        {
            Console.Write("确定要生成密匙吗(Y/N):");
            var k = Console.ReadKey();
            if (k.Key == ConsoleKey.Y)
            {
                var rsa = new RSACryptoServiceProvider(1024);

                File.WriteAllText("publicKey.xml", rsa.ToXmlString(false));
                File.WriteAllText("privateKey.xml", rsa.ToXmlString(true));
            }
            Console.WriteLine();
            Console.WriteLine("Done");
            Console.ReadKey();
        }

        /// <summary>
        /// 生成license
        /// </summary>
        static void GenerateLicense()
        {
            Console.Write("确定要生成许可吗(Y/N):");
            var k = Console.ReadKey();
            if (k.Key == ConsoleKey.Y)
            {
                var privateKey = File.ReadAllText("privateKey.xml");

                var id = Guid.NewGuid();
                var generator = new LicenseGenerator(privateKey);

                Console.WriteLine();
                Console.Write("许可名称: ");
                var name = Console.ReadLine();

                //Console.Write("Date: ");
                //var expirationDate = DateTime.Parse(Console.ReadLine());

                //Console.Write("Type: ");
                //LicenseType licenseType = (LicenseType)Enum.Parse(typeof(LicenseType), Console.ReadLine());

                // generate the license
                var license = generator.Generate(name, id, new DateTime(2020, 12, 31), LicenseType.Standard);

                Console.WriteLine();
                Console.WriteLine(license);
                File.WriteAllText("license.lic", license);
            }
            Console.ReadKey();

        }

        static void TestLicense()
        {
            var publicKey = File.ReadAllText("publicKey.xml");

            Console.Write("测试许可名称:");
            var name = Console.ReadLine();

            try
            {
                new LicenseValidator(publicKey, "license.lic")
                              .AssertValidLicense();

                Console.WriteLine("ok");
            }
            catch (Exception)
            {
                Console.WriteLine("false");
            }

            Console.ReadKey();
        }
    }
}
