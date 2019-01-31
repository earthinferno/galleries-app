import React from 'react';
import Image from './image.jsx';
import AddImage from './add-image.jsx';
import { ImageDataService } from './data-source';

export default class Images extends React.Component{
    constructor(props){
        super(props);

        this.state = {
            images: [],
        }

        this.handleImagesChange = this.handleImagesChange.bind(this);
    }


    componentDidMount() {
        // public int ID { get; set; }
        // [Required]
        // [MaxLength(256)]
        // public string Name { get; set; }
        // [Required]
        // public DateTime CreatedDate { get; set; }
        // [MaxLength(2048)]
        // public string Description { get; set; }
        // [Required]
        // [MaxLength(256)]
        // public string UserId { get; set; }

        // "id": 4,
        // "name": "Gallery C",
        // "createdDate": "2019-01-28T00:00:00",
        // "description": "",
        // "mediaItems": null,
        // "owner": {
        //     "id": 15,
        //     "firstName": "Thomas",
        //     "lastName": "Man",
        //     "emailAddress": "Thomas@thomas.com",
        //     "externalUserId": "Thomas@thomas.com",
        //     "externalIdentityProvider": "Auth0"        

        const data = {
            galleryId: this.props.galleryData.id,
            userId: this.props.userId
        }
        ImageDataService.getImageData(data, imageData => 
          this.setState({images: imageData})
        );
      }
    
    

    handleImagesChange() {
        const data = {
            galleryId: this.props.galleryData.id,
            userId: this.props.userId
        }        
        ImageDataService.getImageData(data, imageData => 
          this.setState({images: imageData})
        );
      }
    
    render(){
        return (
            <div>
                <div>
                  {this.state.images.map( image => (
                    <Image key={image.blobName} onImagesChange={this.handleImagesChange} imageData={image} userId={this.props.userId}/>
                ))}
                </div>
                <p></p>

                <AddImage onImagesChange={this.handleImagesChange} userId={this.props.userId} galleryName={this.props.galleryData.name}/>
            </div>
        )};
}