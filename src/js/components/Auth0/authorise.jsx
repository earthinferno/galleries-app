import React, {Component} from 'react';
import {withRouter} from 'react-router-dom';
import { auth0Client } from '../../globals';
import { Auth0DataService } from './data-source';
//import { sleep } from './../../debug-tools.js';
import AlertMessage from '../modal-message/alert-message.jsx';

class Authorise extends Component {
  constructor(props){
    super(props);
    this.state = {
      loaded: false,
      authoriseationFailureMsg: '',
    }

    this.getUserAccount = this.getUserAccount.bind(this);
    this.handleGetAccount = this.handleGetAccount.bind(this);
    this.onSuccessfulAuthorisation = this.onSuccessfulAuthorisation.bind(this);
    this.onAuthorisationFailure = this.onAuthorisationFailure.bind(this);
    this.handleErrorMessageClosed = this.handleErrorMessageClosed.bind(this);
  }

  componentDidMount() {
    auth0Client.handleAuthentication(this.onSuccessfulAuthorisation, this.onAuthorisationFailure);
  }


  onSuccessfulAuthorisation()
{
    console.log(auth0Client.getProfile());
    this.props.setIdentity(auth0Client.getProfile())
    this.props.setUserId(auth0Client.getProfile().name);

    this.getUserAccount(auth0Client.getProfile().name);
}

  onAuthorisationFailure(err)
  {
    let errMsg = 'Error occured. Details: ' + JSON.stringify(err);
    if (err.error == 'unauthorized')
    {
      errMsg = "Unauthorised user account. Access to this app has been restricted."
    }
    console.log(errMsg);
    this.setState({loaded: true, authoriseationFailureMsg: errMsg})
  }

  handleErrorMessageClosed(){
    this.props.history.replace('/');
  }

  getUserAccount(userId){
    console.log("getUserAccount" + new Date());
    Auth0DataService.getAccount(userId, data => 
         this.handleGetAccount(data), 
        );    
  }
  
  handleGetAccount(data){
    console.log("handleGetAccount"  + new Date());
    this.setState({loaded:true});
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
    const loadingMessage = 
      <div>
        <p>Loading profile...</p>
        <p>Please be patient the page will load. This is running on a free app service plan. </p>
        <p>There is an excessive delay which the developer is investigating....</p>
      </div>;

    const errorMessage = 
      <div>
        <AlertMessage messageBody={this.state.authoriseationFailureMsg}  onMessageClose={this.handleErrorMessageClosed}/>
      </div>;
    
    const pooh = <div>Pooh</div>

    return (
      <div>
        {!this.state.loaded && loadingMessage}
        {this.state.authoriseationFailureMsg != '' && errorMessage}
        {this.state.loaded && pooh}
      </div>
      // <div>
      //   <p>Loading profile...</p>
      //   <p>Please be patient the page will load. This is running on a free app service plan. </p>
      //   <p>There is an excessive delay which the developer is investigating....</p>
      // </div>

    );
  }
}

export default withRouter(Authorise);