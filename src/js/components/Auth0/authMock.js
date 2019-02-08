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
      if (1==1)
      {
        return reject({error: 'unauthorized', errorDescription: 'Access denied.'});
        //reject('blrr');
      }

  
      var now = new Date();
      var expireyTime = new Date(now.getTime() + 1000*60*60*1); //1 hour
      this.idToken = null;
      this.profile = {name:'boris'};;
      this.expiresAt = expireyTime;  
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
  }
}

