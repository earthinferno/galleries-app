import {Route, withRouter} from 'react-router-dom';
import React from 'react';
import Navbar from './navbar.jsx'
//import MainRoutes from './routes.jsx';
import { callback } from '../globals';
//import allback from './Auth0/callback.jsx';
import Images from './galleries-collection/images.jsx';
import Welcome from './welcome-page/welcome.jsx';


export default class App extends React.Component {
    constructor(props){
      super(props);
    }

    render() {
        return (
          <div>
            <Navbar/>
            <Route exact path="/" component={Welcome} />
            <Route exact path='/callback' component={callback}/>
            <Route exact path="/home" render={(props) => <Images {...props} />} />
          </div>
        );
      }


/*
    render() {
      return (
        <div>
            <Navbar history={this.props.history} />
            <MainRoutes history={this.props.history} />
        </div>
      );      
    }
    */
  }