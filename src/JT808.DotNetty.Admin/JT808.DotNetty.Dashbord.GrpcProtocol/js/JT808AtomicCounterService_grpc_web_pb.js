/**
 * @fileoverview gRPC-Web generated client stub for JT808.GrpcDashbord.AtomicCounterGrpcService
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!



const grpc = {};
grpc.web = require('grpc-web');


var ResultReply_pb = require('./ResultReply_pb.js')

var EmptyRequest_pb = require('./EmptyRequest_pb.js')
const proto = {};
proto.JT808 = {};
proto.JT808.GrpcDashbord = {};
proto.JT808.GrpcDashbord.AtomicCounterGrpcService = require('./JT808AtomicCounterService_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServiceClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

  /**
   * @private @const {?Object} The credentials to be used to connect
   *    to the server
   */
  this.credentials_ = credentials;

  /**
   * @private @const {?Object} Options for the client
   */
  this.options_ = options;
};


/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServicePromiseClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServiceClient} The delegate callback based client
   */
  this.delegateClient_ = new proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServiceClient(
      hostname, credentials, options);

};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest,
 *   !proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>}
 */
const methodInfo_AtomicCounterService_GetTcpAtomicCounter = new grpc.web.AbstractClientBase.MethodInfo(
  proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply,
  /** @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply.deserializeBinary
);


/**
 * @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServiceClient.prototype.getTcpAtomicCounter =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterService/GetTcpAtomicCounter',
      request,
      metadata,
      methodInfo_AtomicCounterService_GetTcpAtomicCounter,
      callback);
};


/**
 * @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>}
 *     The XHR Node Readable Stream
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServicePromiseClient.prototype.getTcpAtomicCounter =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.getTcpAtomicCounter(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest,
 *   !proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>}
 */
const methodInfo_AtomicCounterService_GetUdpAtomicCounter = new grpc.web.AbstractClientBase.MethodInfo(
  proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply,
  /** @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply.deserializeBinary
);


/**
 * @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>|undefined}
 *     The XHR Node Readable Stream
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServiceClient.prototype.getUdpAtomicCounter =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterService/GetUdpAtomicCounter',
      request,
      metadata,
      methodInfo_AtomicCounterService_GetUdpAtomicCounter,
      callback);
};


/**
 * @param {!proto.JT808.GrpcDashbord.ServiceGrpcBase.EmptyRequest} request The
 *     request proto
 * @param {!Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterReply>}
 *     The XHR Node Readable Stream
 */
proto.JT808.GrpcDashbord.AtomicCounterGrpcService.AtomicCounterServicePromiseClient.prototype.getUdpAtomicCounter =
    function(request, metadata) {
  return new Promise((resolve, reject) => {
    this.delegateClient_.getUdpAtomicCounter(
      request, metadata, (error, response) => {
        error ? reject(error) : resolve(response);
      });
  });
};


module.exports = proto.JT808.GrpcDashbord.AtomicCounterGrpcService;

