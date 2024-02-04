using System;
using System.IO.Ports;

class Program
{
    static void Main()
    {
        string[] ports = SerialPort.GetPortNames();

        if (ports.Length > 0)
        {
            Console.WriteLine("Available serial ports:");
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }
        else
        {
            Console.WriteLine("No serial ports found.");
        }
    }
}