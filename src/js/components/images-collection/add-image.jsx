import React from  'react';
import { ImageDataService } from './data-source'

export default class AddImage extends React.Component{
  constructor(props)
  {
    super(props);

    this.state = {
      Comment: '',
  };
  
    this.handleInputChange = this.handleInputChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.fileInput = React.createRef();
    this.form = React.createRef;
  }



  handleSubmit(event)
  {
    event.preventDefault();

    var mediaFiles = new FormData(this.form);
    mediaFiles.append('File',this.fileInput.current.files[0]);
    mediaFiles.append('Comment',this.state.Comment);
    mediaFiles.append('UserFolder',this.props.galleryName);
    mediaFiles.append('UserId',this.props.userId);


    ImageDataService.addImage(mediaFiles, data => 
      this.props.onImagesChange());
  }

  handleInputChange()
  {
      const target = event.target;
      const value = target.value;
      const name = target.name;
  
      this.setState({
        [name]: value
      });
  }


  render(){
    return (
      <form onSubmit={this.handleSubmit} ref={this.form}>
        <label> Upload file: 
          <input type="file" ref={this.fileInput} />
        </label>
        <div>Comment: <input name='Comment' type='text' value={this.state.Comment} onChange={this.handleInputChange} /></div>

        <button type="submit">Upload Image</button>      
    </form>
    );
  }
}

