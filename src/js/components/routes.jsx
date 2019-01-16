import React from 'react';
import { Route } from 'react-router-dom';
import Images from './galleries-collection/images.jsx';
import Welcome from './welcome-page/welcome.jsx';
import Callback from './Auth0/callback.jsx';
import { auth0Client } from '../globals';


export default class MainRoutes extends React.Component {
  constructor(props)
  {
    super(props);
    this.handleAuthentication = this.handleAuthentication.bind(this);
 
  }

/* example
      <div className="content">
          <Route exact path="/" component={Home} />
          <Route exact path="/marketing" component={Marketing} />
          <Route exact path="/automation" component={Automation} />
      </div>
*/

  handleAuthentication (props, replace) 
  {
    if (/access_token|id_token|error/.test(nextState.location.hash)) 
    {
      auth0Client.handleAuthentication(props.history);
    }
  }
  //<Route exact path="/home" render={(props) => <Images auth={auth} {...props} />} />
//          <Route exact path="/callback" render={(props) => {  this.handleAuthentication(props);    return <Callback {...props} />  }} />
  render() {
    return (
      <div className="content">
          <Route exact path="/" component={Welcome} />
          <Route exact path="/home" render={(props) => <Images {...props} />} />
          <Route exact path="/callback" render={(props) => {  this.handleAuthentication(props);    return <Callback {...props} />  }} />

      </div>

      );      
  }
}


