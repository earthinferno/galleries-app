import React, {Component} from 'react';
import {withRouter} from 'react-router-dom';
import { auth0Client } from '../../globals';

class Callback extends Component {
  async componentDidMount() {
    await auth0Client.handleAuthentication();
    this.props.history.replace('/home');
  }

  render() {
    return (
      <p>Loading profile...</p>
    );
  }
}

export default withRouter(Callback);