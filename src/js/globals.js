// Imports here
import AuthMock from './components/Auth0/authMock';
import Auth from './components/Auth0/auth';

// Global objects here


// Conditional objects here
let auth0Client = null;
let baseuri = "";
let baseapiurl = "https://galleriesserver247.azurewebsites.net";

if (process.env.NODE_ENV === 'development') {
    baseuri = "http://localhost:8080";
    baseapiurl = "http://localhost:60782";
    auth0Client = new AuthMock(baseuri);
    
    
} else if (process.env.NODE_ENV == 'production') {
    baseuri = "https://galleries247b.azurewebsites.net";
    baseapiurl = "https://galleriesserver247.azurewebsites.net";
    auth0Client = new Auth(baseuri);
    
}

// exports
export { auth0Client} ;
export { baseuri, baseapiurl} ;

