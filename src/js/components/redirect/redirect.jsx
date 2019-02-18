import React, { Component } from "react";

export class Redirect extends Component {
  constructor( props ){
    super();
    this.state = {location: props.loc };
  }
  componentWillMount(){
    window.location = this.state.location;
  }
  render(){
    return (<section>Redirecting...</section>);
  }
}

export default Redirect;