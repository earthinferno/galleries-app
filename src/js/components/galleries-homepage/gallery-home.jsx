import React from 'react';
import AddGallery from './add-gallery.jsx';
import GalleryList from './gallery-list.jsx';
import { GalleryDataService } from './data-source';

export default class GalleryHome extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            UserId: this.props.UserId,
            galleries: []
        }

        this.refreshGalleryData = this.refreshGalleryData.bind(this);

    }

    componentDidMount() {
        GalleryDataService.getGalleriesData(this.state.UserId, galleryData => 
            this.setState({galleries: galleryData})
        );
        }
        
    refreshGalleryData(data) {
        GalleryDataService.getGalleriesData(this.props.UserId, galleryData => 
            this.setState({galleries: galleryData})
        );
    }

    render(){
        return (
            <div>
                <GalleryList galleries={this.state.galleries} />
                <AddGallery onAddGallery={this.refreshGalleryData} UserId={this.props.UserId}/>
            </div>
            
            
        );
    }
}
