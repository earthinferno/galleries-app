import React from 'react';
import Image from './image.jsx';
import { ImageDataService } from './data-source'

export class Images extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      images : ImageDataService.getImageData(),
    }
  }

  render() {
    if (this.state.images == null)
    {
      return(null);
    }

    return (
        <div>
          {this.state.images.map( image => (
            <Image key={image.id} url={image.url} comment={image.comment} like={image.liked}/>
          ))}
        </div>
    );      
  }
}
