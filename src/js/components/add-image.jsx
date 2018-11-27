import React from  'react';

export default class AddImage extends React.Component{
  constructor(props)
  {
    super(props);
    //this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.fileInput = React.createRef();
  }

  handleSubmit(event)
  {
    event.preventDefault();
    console.log(this.fileInput.current.files[0].name);
    //console.log(this.fileInput.current.files[0].);
  }

  handleChange(fileList)
  {
    console.log(fileList);
  }
  //<input type="file" onChange={ (e) => this.handleChange(e.target.files)}/>
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

