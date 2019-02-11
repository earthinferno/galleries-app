import auth0 from 'auth0-js';

export default class Auth {
  constructor(uri) {
    this.auth0 = new auth0.WebAuth({
      // the following three lines MUST be updated
      domain: '88galleries88state.eu.auth0.com',
      audience: 'https://88galleries88state.eu.auth0.com/userinfo',
      clientID: 'ySDhFb8l20IORZItgTf5v5AjVfbU6WVj',
      redirectUri: uri + '/authorise',
      responseType: 'token id_token',
      scope: 'openid profile'
    });

    this.getProfile = this.getProfile.bind(this);
    this.handleAuthentication = this.handleAuthentication.bind(this);
    this.isAuthenticated = this.isAuthenticated.bind(this);
    this.login = this.signIn.bind(this);
    this.logout = this.signOut.bind(this);
    this.setSession = this.setSession.bind(this);
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
    this.auth0.authorize();
  }

  handleAuthentication(successCallback, errorCallback) {
      let promise =  new Promise((resolve, reject) => {
        this.auth0.parseHash((err, authResult) => {
          if (err) return reject(err);
          if (!authResult || !authResult.idToken) {
            return reject(err);
          }
          // this.idToken = authResult.idToken;
          // this.profile = authResult.idTokenPayload;
          // // set the time that the id token will expire at
          // this.expiresAt = authResult.idTokenPayload.exp * 1000;
          this.setSession(authResult);
          resolve();
        });
      })      
      promise.then(successCallback, error => errorCallback(error));
      console.log("handleAuthentication");
      return promise;
  }

  signOut() {
    // // clear id token, profile, and expiration
    // this.idToken = null;
    // this.profile = null;
    // this.expiresAt = null;

    // Remove tokens and expiry time
    this.accessToken = null;
    this.idToken = null;
    this.expiresAt = 0;
    this.profile = null;

    // Remove isLoggedIn flag from localStorage
    localStorage.removeItem('isLoggedIn');    
  }

  setSession(authResult) {
    // Set isLoggedIn flag in localStorage
    localStorage.setItem('isLoggedIn', 'true');

    // Set the time that the access token will expire at
    //let expiresAt = (authResult.idTokenPayload.exp * 1000) + new Date().getTime();
    let expiresAt = (authResult.expiresIn /** 1000*/) + new Date().getTime();
    this.expiresAt = expiresAt;
    this.accessToken = authResult.accessToken;
    this.idToken = authResult.idToken;
    this.profile = authResult.idTokenPayload;


  }
}


