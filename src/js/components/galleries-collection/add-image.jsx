import React from  'react';
import { ImageDataService } from '../data-source'

export default class AddImage extends React.Component{
  constructor(props)
  {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.fileInput = React.createRef();
  }

  handleSubmit(event)
  {
    event.preventDefault();

    var files = new FormData(this.form);
    files.append('Files[]',this.fileInput.current.files[0]);
    
    // ImageDataService.addImage(files, data => 
    // this.setState({images: data}));
    ImageDataService.addImage(files, data => 
      this.props.onImagesChange(data));
  }

  render(){
    return (
      <form onSubmit={this.handleSubmit}>
        <label> Upload file: 
          <input type="file" ref={this.fileInput} />
        </label>
        <br/>
        <button type="submit">Upload Image</button>      
    </form>
    );
  }
}

