//Used when running webpack-dev server which roesn't support auth0 sign in
//import auth0 from 'auth0-js';

export default class Auth {
  constructor(uri) {
    this.uri = uri + "/authorise";
    this.getProfile = this.getProfile.bind(this);
    this.handleAuthentication = this.handleAuthentication.bind(this);
    this.isAuthenticated = this.isAuthenticated.bind(this);
    this.login = this.signIn.bind(this);
    this.logout = this.signOut.bind(this);
  }

  getProfile() {
    return this.profile;
  }

  getIdToken() {
    return this.idToken;
  }

  isAuthenticated() {
    return new Date().getTime() < this.expiresAt;
  }

  signIn(history) {
    
    history.replace('/authorise');
  }

  handleAuthentication(successCallback, errorCallback) {
    let promise =  new Promise((resolve, reject) => {
      if (1==2)
      {
        return reject({error: 'unauthorized', errorDescription: 'Access denied.'});
      }
  
      let authResult = {
        accessToken: 111,
        idToken: null,
        idTokenPayload: {
            // what value should this be?
            exp: 1, 
            name: 'boris'
        }
      }
      this.setSession(authResult);
      //var now = new Date();
      //var expireyTime = new Date(now.getTime() + 1000*60*60*1); //1 hour
      // this.idToken = null;
      // this.profile = {name:'boris'};;
      // this.expiresAt = expireyTime;  
      resolve();
    });
    promise.then(successCallback, error => errorCallback(error));
    return promise;
  }

  signOut() {
    // clear id token, profile, and expiration
    this.idToken = null;
    this.profile = null;
    this.expiresAt = null;
    this.profile = null;

    // Remove isLoggedIn flag from localStorage
    localStorage.removeItem('isLoggedIn');        
  }

  setSession(authResult) {
    // Set isLoggedIn flag in localStorage
    localStorage.setItem('isLoggedIn', 'true');

    // Set the time that the access token will expire at
    let expiresAt = new Date(now.getTime() + 1000*60*60*1);
    this.accessToken = authResult.accessToken;
    this.idToken = authResult.idToken;
    this.profile = authResult.idTokenPayload;
    this.expiresAt = expiresAt;

  }  
}

