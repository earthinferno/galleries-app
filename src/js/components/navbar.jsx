import React from 'react';
//import { Navbar, Button } from 'react-bootstrap';
import {Link, withRouter} from 'react-router-dom';
import { auth0Client } from '../globals';

function NavBar(props) {
    const signOut = () => {
        auth0Client.logout();
        props.history.replace('/');
    };

    const login = () => {
        auth0Client.login(props.history);
    };

    return (
        <nav className="navbar navbar-dark bg-primary fixed-top">
        <Link className="navbar-brand" to="/">
            Bottoms
        </Link>
        {
            !auth0Client.isAuthenticated() &&
            <button className="btn btn-dark" onClick={() => {login()}}>Sign In</button>
        }
        {
            auth0Client.isAuthenticated() &&
            <div>
            <label className="mr-2 text-white">{auth0Client.getProfile().name}</label>
            <button className="btn btn-dark" onClick={() => {signOut()}}>Sign Out</button>
            </div>
        }
        </nav>
    );
}

export default withRouter(NavBar);

