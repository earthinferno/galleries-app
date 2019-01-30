import auth0 from 'auth0-js';

export default class Auth {
  constructor(uri) {
    this.auth0 = new auth0.WebAuth({
      // the following three lines MUST be updated
      domain: '88galleries88state.eu.auth0.com',
      audience: 'https://88galleries88state.eu.auth0.com/userinfo',
      clientID: 'ySDhFb8l20IORZItgTf5v5AjVfbU6WVj',
      redirectUri: uri + '/authorise',
      responseType: 'id_token',
      scope: 'openid profile'
    });

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
    this.auth0.authorize();
  }

  handleAuthentication() {
      return new Promise((resolve, reject) => {
        this.auth0.parseHash((err, authResult) => {
          if (err) return reject(err);
          if (!authResult || !authResult.idToken) {
            return reject(err);
          }
          this.idToken = authResult.idToken;
          this.profile = authResult.idTokenPayload;
          // set the time that the id token will expire at
          this.expiresAt = authResult.idTokenPayload.exp * 1000;
          resolve();
        });
      })      
  }

  signOut() {
    // clear id token, profile, and expiration
    this.idToken = null;
    this.profile = null;
    this.expiresAt = null;
  }
}

//const auth0Client = new Auth();
//export default auth0Client;


