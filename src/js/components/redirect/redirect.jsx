import React, { Component } from "react";

export class Redirect extends Component {
  constructor( props ){
    super();
    this.state = {
      location: props.loc,
      nextLocation: props.nextloc,
    };
  }
  componentWillMount(){
    window.location = this.state.location;
  }

  componentWillMount(){
    this.props.history.replace(this.state.nextLocation);
    console.log('redirect=/');
  }
  render(){
    return (<section>Redirecting...</section>);
  }
}

export default Redirect;