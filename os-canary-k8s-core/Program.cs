using System;
using System.Net;

namespace os_canary_k8s_core
{
    internal class Program
    {
        static string os, home, hostname, imagename, appversion, pod_name, pod_namespace;
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing...");
            Init_EnvVars();
            Log_EnvVars();


            //Based heavily from this: https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener?view=net-6.0&WT.mc_id=DT-MVP-5002999
            string listeningUri = "http://*:58081/";  // Using wildcard listener is probably a bad security practice, but ffs this isn't really doing anything
            Console.WriteLine($"HTTP server will listen as: {listeningUri}");

            HttpListener listener = new System.Net.HttpListener();
            listener.Prefixes.Add(listeningUri);
            listener.Start();
            Console.WriteLine("Listening");


            // Putting this shit in a loop because I'm too stupid to grasp the async callbacks and stuff
            // re: https://docs.microsoft.com/en-us/dotnet/api/system.net.httplistener.begingetcontext?view=net-6.0
            while (true)
            {
                ListenForReq(listener);
            }

            listener.Stop();
        }

        static void Init_EnvVars()
        {
            // Env var checks
            Console.WriteLine("Fetching env vars");
            os = Environment.GetEnvironmentVariable("OS");
            home = Environment.GetEnvironmentVariable("HOME");
            hostname = Environment.GetEnvironmentVariable("HOSTNAME");
            imagename = Environment.GetEnvironmentVariable("IMAGENAME");
            appversion = Environment.GetEnvironmentVariable("APPVERSION");
            pod_name = Environment.GetEnvironmentVariable("POD_NAME");
            pod_namespace = Environment.GetEnvironmentVariable("POD_NAMESPACE");
        }

        static void Log_EnvVars()
        {
            Console.WriteLine("os :: " + os);
            Console.WriteLine("home :: " + home);
            Console.WriteLine("hostname :: " + hostname);
            Console.WriteLine("imagename :: " + imagename);
            Console.WriteLine("appversion :: " + appversion);
            Console.WriteLine("pod_name :: " + pod_name);
            Console.WriteLine("pod_namespace :: " + pod_namespace);
        }

        static void ListenForReq(HttpListener listener)
        {

            // GetContext blocks while waiting for a req
            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            Console.WriteLine("Processing request");
            // Obtain a response object
            HttpListenerResponse response = context.Response;
            response.ContentType = "application/json";

            string responseString = "{\"msg\":\"hello there\",\"envData\":{\"hostname\":\"" + hostname + "\",\"podName\":\"" + pod_name + "\", \"podNamespace\":\"" + pod_namespace + "\"},\"appData\":{\"imageName\":\"" + imagename + "\",\"appVersion\":\"" + appversion + "\"}}";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}