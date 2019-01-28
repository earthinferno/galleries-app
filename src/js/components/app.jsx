import {Route, withRouter} from 'react-router-dom';
import React from 'react';
import Navbar from './navbar.jsx'
import { callback } from '../globals';
import Images from './galleries-collection/images.jsx';
import GalleryHome from './galleries-homepage/gallery-home.jsx';
import Welcome from './welcome-page/welcome.jsx';


export default class App extends React.Component {
    constructor(props){
      super(props);
    }

    //<Route exact path="/home" render={(props) => <Images {...props} />} />
    render() {
        return (
          <div>
            <Navbar/>
            <Route exact path="/" component={Welcome} />
            <Route exact path='/callback' component={callback}/>
            <Route exact path="/home" render={(props) => <GalleryHome UserId="Thomas@thomas.com" {...props} />} />
          </div>
        );
      }

  }