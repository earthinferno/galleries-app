import React from 'react';
import AddGallery from './add-gallery.jsx';
import GalleryList from './gallery-list.jsx';
import { GalleryDataService } from './data-source';

export default class GalleryHome extends React.Component {
    constructor(props)
    {
        super(props);
        this.state = {
            userId: this.props.userId,
            galleries: []
        }

        this.refreshGalleryData = this.refreshGalleryData.bind(this);

    }

    componentDidMount() {
        console.log("GalleryHome:componentDidMount"  + new Date());
        GalleryDataService.getGalleriesData(this.state.userId, galleryData => 
            this.setState({galleries: galleryData})
        );
        }
        
    refreshGalleryData(data) {
        GalleryDataService.getGalleriesData(this.props.userId, galleryData => 
            this.setState({galleries: galleryData})
        );
    }

    render(){
        return (
            <div>
                <GalleryList galleries={this.state.galleries} userId={this.props.userId} {...this.props}/>
                <AddGallery onAddGallery={this.refreshGalleryData} userId={this.props.userId} />
            </div>
            
            
        );
    }
}
