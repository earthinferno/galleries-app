import React from 'react';
import Image from './image.jsx';

export default class Images extends React.Component {

  render() {
    const images = this.props.images;
    if (images == null)
    {
      return(null);
    }

    return (
        <div>
          {images.map( image => (
            <Image key={image.id} url={image.url} comment={image.comment} like={image.liked}/>
          ))}
        </div>
    );      
  }
}