  import {baseapiurl} from '../globals';
  
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
  }

  export { ImageDataService }