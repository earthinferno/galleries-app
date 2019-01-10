  import {baseapiurl} from '../config/settings.js';
  
  class ImageDataService {
    static getImageDatStub() {
      this.imageId = 0;
      this.imageData = 
      [
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        },
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        },
        {
          id: this.imageId++,
          url: "./../../src/img/index.png",
          comment: "First Image",
          liked: false,
        }
      ]

      return this.imageData;
    }

    static getImageData(callback) {
      $.get(baseapiurl + "/api/images", function(data, status){
        callback(data.results);
      });
    }

    static addImage(data, callback) {
      //https://localhost:44320/
      //'http://localhost:60782/api/images

      $.ajax({
        url: baseapiurl + '/api/images',
        data: data,
        cache: false,
        contentType: false,
        processData: false,
        method: 'Post',
        success:  function(data, status){
          callback(data.results);
        }
      });
    }
  }

  export { ImageDataService }