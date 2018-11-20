import React from "react";

export default class LikeButton extends React.Component {
  constructor(props) {
      super(props);
      this.state = {
          liked: false,
      };
  }
  likedLabel() {
      if (this.state.liked) {
          return 'Liked';
      }
      else{
        return 'Like this';
      }
  }
  render() {
    return (
        <button onClick={() => this.setState({ liked: true})}>
            {this.likedLabel()}
        </button>
    );      
  }
}