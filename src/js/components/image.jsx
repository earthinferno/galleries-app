import React from 'react';
import LikeButton from './like-button.jsx';

export default class Image extends React.Component {
  render() {
    return (
        <div>
            <img src={this.props.url}></img>
            <div>
              <div>Comment: {this.props.comment}</div>
              <LikeButton like={true} />
            </div>
        </div>
    );      
  }
}