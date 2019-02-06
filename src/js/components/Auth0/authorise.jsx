import React, {Component} from 'react';
import {withRouter} from 'react-router-dom';
import { auth0Client } from '../../globals';
import { Auth0DataService } from './data-source';
//import { sleep } from './../../debug-tools.js';

class Authorise extends Component {
  constructor(props){
    super(props);

    this.getUserAccount = this.getUserAccount.bind(this);
    this.handleGetAccount = this.handleGetAccount.bind(this);
    this.onSuccessfulAuthorisation = this.onSuccessfulAuthorisation.bind(this);
  }

  async componentDidMount() {
    await auth0Client.handleAuthentication(this.onSuccessfulAuthorisation);

  }

  onSuccessfulAuthorisation()
{
    console.log(auth0Client.getProfile());
    this.props.setIdentity(auth0Client.getProfile())
    this.props.setUserId(auth0Client.getProfile().name);


    this.getUserAccount(auth0Client.getProfile().name);
}

  getUserAccount(userId){
    console.log("getUserAccount");
    Auth0DataService.getAccount(userId, data => 
         this.handleGetAccount(data), 
        );    
  }
  
  handleGetAccount(data){
    console.log("handleGetAccount");
    //sleep(1000); 
    // Was an account return. If not register the account details   
    if (data== null)
    {
      this.props.history.replace('/register');
    }
    else
    {
      this.props.history.replace('/home');
    }
  }

  render() {
    return (
      <p>Loading profile...</p>
    );
  }
}

export default withRouter(Authorise);