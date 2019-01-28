  import {baseapiurl} from '../../globals';
  
  class GalleryDataService {
    static getGalleriesData(userId, callback){
      $.get(baseapiurl + "/api/galleries/" + userId, function(data, status){
        callback(data);
      });      
    }

    // static addGallery(data, callback){
    //   $.post(baseapiurl + "/api/galleries",data,  function(data, status){
    //     callback(data);
    //   },
    //   'application/json'
    //   );
    // }    

    static addGallery(data, callback){
      $.ajax({
        url: baseapiurl + '/api/galleries',
        dataType: 'json',
        method: 'Post',
        contentType: 'application/json',
        cache: false,
        data: JSON.stringify(data),
        processData: false,
        success:  function(data, status){
          callback(data);
        }
      });
  
    } 
    

/*

    static getImageData(callback) {
      $.get(baseapiurl + "/api/images", function(data, status){
        callback(data.results);
      });
    }

    static addImage(files, metatadata, callback) {
      $.ajax({
        url: baseapiurl + '/api/images',
        data: files,
        cache: false,
        contentType: false,
        processData: false,
        method: 'Post',
        success:  function(data, status){
          callback(data.results);
        }
      });
    }
*/    
  
}

  export { GalleryDataService }