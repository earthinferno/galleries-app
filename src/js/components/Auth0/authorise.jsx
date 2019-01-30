import React, {Component} from 'react';
import {withRouter} from 'react-router-dom';
import { auth0Client } from '../../globals';
import { Auth0DataService } from './data-source';
import Register from '../register/register.jsx';

class Authorise extends Component {
  constructor(props){
    super(props);

    this.getUserAccount = this.getUserAccount.bind(this);
    this.handleGetAccount = this.handleGetAccount.bind(this);
  }

  async componentDidMount() {
    await auth0Client.handleAuthentication();

    console.log(auth0Client.getProfile());
    this.props.setIdentity(auth0Client.getProfile())
    this.props.setUserId(auth0Client.getProfile().name);

    this.getUserAccount(auth0Client.getProfile().name);
    this.props.history.replace('/home');
  }

  getUserAccount(userId){
    Auth0DataService.getAccount(userId, data => 
         this.handleGetAccount(data), 
        );    
  }
  
  handleGetAccount(data){
    if (data== null)
    {
      this.props.history.replace('/register');
    }
  }

  render() {
    return (
      <p>Loading profile...</p>
    );
  }
}

export default withRouter(Authorise);