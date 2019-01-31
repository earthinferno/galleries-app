import React from 'react';
import Images from '../images-collection/images-container.jsx';

export default class GallerySummary extends React.Component {
    constructor(props){
        super(props);
        this.state = {
            OpenGalleryExpanded: false,
        };

        this.handleOpenGallery = this.handleOpenGallery.bind(this);

    }

    handleOpenGallery(event){
        let expanded = !this.state.OpenGalleryExpanded;
        this.setState({OpenGalleryExpanded: expanded});

        event.preventDefault();
    }

    render(){
        const expanded = this.state.OpenGalleryExpanded;
        return (
            <li>
                <p>
                    <span>Gallery Name: {this.props.data.name}</span>
                    <span>Create Date: {this.props.data.createdDate}</span>
                    <span>Description: {this.props.data.description}</span>
                    <span>Owner name: {this.props.data.owner.firstName + " " + this.props.data.owner.lastName}</span>
                </p>
                <button onClick={this.handleOpenGallery}>{expanded ? 'Close' : 'Open Gallery'}</button>
                {expanded && 
                    <Images galleryData={this.props.data} userId={this.props.userId}/>
                }
                
            </li>
        );
    }
}
