namespace Klient 
{
    using NServiceBus;

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
	    public EndpointConfig()
	    {
            // Mulighet for konfigurerering
	    }
    }
}