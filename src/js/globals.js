// Imports here
import AuthMock from './components/Auth0/authMock';
import Auth from './components/Auth0/auth';

// Global objects here
const logouturi = "https://88galleries88state.eu.auth0.com/v2/logout";

// Conditional objects here
let auth0Client = null;
//let baseuri = "";
//let baseapiurl = "https://galleriesserver247.azurewebsites.net";


// if (process.env.NODE_ENV === 'development') {
//     baseuri = "http://localhost:8080";
//     baseapiurl = "http://localhost:60782";
//     auth0Client = new AuthMock(baseuri);
    
// } 
// if (process.env.NODE_ENV === 'production') {
//     baseuri = "https://galleries247b.azurewebsites.net";
//     baseapiurl = "https://galleriesserver247.azurewebsites.net";
//     auth0Client = new Auth(baseuri);
    
// } 
// if (process.env.env === 'preprod') {
//     baseuri = "https://galleries-dev-client.azurewebsites.net";
//     //baseuri = "YYYYYYYYYYYYYYYYYYYY";
//     baseapiurl = "https://galleries-dev-server.azurewebsites.net";
//     auth0Client = new Auth(baseuri);
// }

const baseuri = process.env.BASE_URI;
const baseapiurl = process.env.BASE_API_URI;
if (process.env.ENVIRONMENT === 'local')
{
    auth0Client = new AuthMock(baseuri);
    console.log("Authenticator=Mock Authenticator");
}
else
{
    auth0Client = new Auth(baseuri);
    console.log("Authenticator=Auth0Client");
}

console.log("baseuri=" + baseuri);
console.log("baseapiurl=" + baseapiurl);

// exports
export { auth0Client, logouturi} ;
export { baseuri, baseapiurl} ;


