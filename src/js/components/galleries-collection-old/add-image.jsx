import React from  'react';
import { ImageDataService } from '../images-collection/data-source'

export default class AddImage extends React.Component{
  constructor(props)
  {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.fileInput = React.createRef();
    this.form = React.createRef;
  }

  handleSubmit(event)
  {
    event.preventDefault();

    var mediaFiles = new FormData(this.form);
    mediaFiles.append('File',this.fileInput.current.files[0]);
    mediaFiles.append('Comment','This is my comment');

    //var mediaFiles = new FormData(this.form);


    var metadata = {
      comment: 'this is a comment'
    }    
    
    // ImageDataService.addImage(files, data => 
    // this.setState({images: data}));
    ImageDataService.addImage(mediaFiles, metadata, data => 
      this.props.onImagesChange(data));
  }

  render(){
    return (
      <form onSubmit={this.handleSubmit} ref={this.form}>
        <label> Upload file: 
          <input type="file" ref={this.fileInput} />
        </label>
        <br/>
        <button type="submit">Upload Image</button>      
    </form>
    );
  }
}

