import React from 'react';
import Image from './image.jsx';
import AddImage from './add-image.jsx';
import { ImageDataService } from '../images-collection/data-source'

export default class Images extends React.Component {

  constructor(props) {
    super(props);
    this.state = {
      images : this.ImageData,
    }

    this.handleImagesChange = this.handleImagesChange.bind(this);
  }

  componentDidMount() {
    ImageDataService.getImageData(imageData => 
      this.setState({images: imageData})
    );
  }

  handleImagesChange(imageData) {
    this.setState({images: imageData});
  }

  render() {
    if (this.state.images == null)
    {
      return(null);
    }

    return (
      <div>
        <div>
          {this.state.images.map( image => (
            <Image key={image.id} url={image.url} comment={image.comment} like={image.liked}/>
          ))}
        </div>
        <p></p>
        <AddImage onImagesChange={this.handleImagesChange} />
      </div>
    );      
  }
}
