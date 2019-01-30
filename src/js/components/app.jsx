import {Route, withRouter} from 'react-router-dom';
import React from 'react';
import Navbar from './navbar.jsx'
import Authorise from './Auth0/authorise.jsx';
import GalleryHome from './galleries-collection/gallery-home.jsx';
import Welcome from './welcome-page/welcome.jsx';
import Register from './register/register.jsx';


export default class App extends React.Component {
    constructor(props){
      super(props);

      // Store information about the user here. Only set on authorisation.
      this.state = {
        userId: '',
        indentity: {}
      }

      this.setUserId = this.setUserId.bind(this);
      this.setIndentity = this.setIndentity.bind(this);
    }

    setUserId(userId){
      this.setState({userId: userId});
    }

    setIndentity(profile, userId){
      this.setState({indentity: profile})
      if (userId != null) {
        this.setState({userId: userId});
      }
    }

    // The available routes for the main panell are defined below. 
    // Navigation is welcome -> callback -> register(optional) -> home
    render() {
        return (
          <div>
            <Navbar/>
            <Route exact path="/" component={Welcome} />
            <Route exact path='/authorise' render={(props) => <Authorise setUserId={this.setUserId} setIdentity={this.setIndentity} {...props}/>}/>
            <Route exact path="/home" render={(props) => <GalleryHome userId={this.state.userId} {...props} />} />
            <Route exact path="/register" render={(props) => <Register identityProfile={this.state.indentity}  userId={this.state.userId} {...props}/>} />
          </div>
        );
      }

  }