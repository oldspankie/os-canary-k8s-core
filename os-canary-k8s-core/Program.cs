using System;
using System.Net;

namespace os_canary_k8s_core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Based heavily from this: https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=net-6.0&WT.mc_id=DT-MVP-5002999

            string listeningUri = "http://*:58081/";  // Using wildcard listener is probably a bad security practice, but ffs this isn't really doing anything

            Console.WriteLine($"HTTP server listening as: {listeningUri}");

            HttpListener listener = new System.Net.HttpListener();
            listener.Prefixes.Add(listeningUri);
            listener.Start();
            Console.WriteLine("Listening");

            // Putting this shit in a loop because I'm too stupid to grasp the async callbacks and stuff
            // re: https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener.begingetcontext?view=net-6.0
            while (true)
            {
                // GetContext blocks while waiting for a req
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Console.WriteLine("Processing request");
                // Obtain a response object
                HttpListenerResponse response = context.Response;
                response.ContentType = "application/json";

                string responseString = "{\"msg\":\"hello there\"}";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            listener.Stop();
        }
    }
}