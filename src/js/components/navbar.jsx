import React from 'react';
import {Link, withRouter} from 'react-router-dom';
import { auth0Client } from '../globals';
import mainlogo from './../../images/main-logo.png';
import welcome from './../../images/welcome-message.png';

function NavBar(props) {
    const signOut = () => {
        auth0Client.logout();
        props.history.replace('/logout')
        props.history.replace('/');
    };

    const login = () => {
        auth0Client.login(props.history);
    };

    return (
        <nav className="navbar ">

            <span className=""><img src={welcome} className="img-fluid custom_image_size" /></span>
            <span>
                <div className="navbar-text text-white custom_heading_size_h30"> All your pictures in one place </div>
                <div className="container">
                    <div className="row justify-content-around">
                        <div className="col-2">                    
                            {
                                !auth0Client.isAuthenticated() &&
                                        <button className="btn btn-outline-primary" onClick={() => {login()}}>Login</button>
                            }
                            {
                                auth0Client.isAuthenticated() &&
                                            <button className="btn btn-outline-primary" onClick={() => {signOut()}}>Logout</button>
                            }
                        </div>
                    </div>
                </div>
            </span>
            <span className="navbar-brand"><img src={mainlogo} className="custom_image_size" /></span>    
            
        </nav>
    );
}

export default withRouter(NavBar);

