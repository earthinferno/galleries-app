import {Route, withRouter} from 'react-router-dom';
import React from 'react';
import Navbar from './navbar.jsx'
import Authorise from './Auth0/authorise.jsx';
import GalleryHome from './galleries-collection/gallery-home.jsx';
import Welcome from './welcome-page/welcome.jsx';
import Register from './register/register.jsx';
import Images from './images-collection/images-container.jsx';
import Redirect from './redirect/redirect.jsx';
import {baseapiurl, logouturi} from '../globals';


export default class App extends React.Component {
    constructor(props){
      super(props);

      // Store information about the user here. Only set on authorisation.
      this.state = {
        userId: '',
        indentity: {},
        galleryData: {},
      }

      this.setUserId = this.setUserId.bind(this);
      this.setIndentity = this.setIndentity.bind(this);
      this.setGalleryData = this.setGalleryData.bind(this);
    }

    componentDidMount() {
      fetch(baseapiurl + '/api/wakeup')
      .then(function(response) {
        return response.json();
      })
      .then(function(myJson) {
        console.log(JSON.stringify(myJson));
      });      
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

    setGalleryData(data){
      this.setState({galleryData: data})
    }

    // The available routes for the main panell are defined below. 
    // Navigation is welcome -> callback -> register(optional) -> home
    render() {
        return (
          <div >
            <Navbar/>
            <Route exact path="/" component={Welcome} />
            <Route exact path='/authorise' render={(props) => <Authorise setUserId={this.setUserId} setIdentity={this.setIndentity} {...props}/>}/>
            <Route exact path="/register" render={(props) => <Register identityProfile={this.state.indentity}  userId={this.state.userId} identityProvider='auth0'  {...props}/>} />
            <Route exact path="/home" render={(props) => <GalleryHome userId={this.state.userId} setGalleryData={this.setGalleryData} {...props} />} />
            <Route exact path="/home/gallery" render={(props) => <Images galleryData={this.state.galleryData} userId={this.state.userId} {...props} />} />
            <Route exact path="/logout" render={(props) => <Redirect loc={logouturi} {...props} />} />

            {/* <div className="fixed-bottom">...</div> */}
          </div>
        );
      }

  }