// Imports here
import AuthMock from './components/Auth0/authMock';
import Auth from './components/Auth0/auth';

// Global objects here
const logouturi = "https://88galleries88state.eu.auth0.com/v2/logout";

// Conditional objects here
let auth0Client = null;

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


