$cli = new-object System.Net.WebClient
$cli.DownloadFile("http://the.earth.li/~sgtatham/putty/latest/x86/pscp.exe", "scp.exe")
