import React from 'react';
import LikeButton from './like-button.jsx';
import imagefile from '../../../images/index.png';
//<img src={this.props.url}></img>
export default class Image extends React.Component {
  render() {
    return (
        <div>
            <img src={imagefile}></img>
            <div>
              <div>Comment: {this.props.comment}</div>
              <LikeButton like={true} />
            </div>
        </div>
    );      
  }
}