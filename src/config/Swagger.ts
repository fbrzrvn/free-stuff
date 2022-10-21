import { validationMetadatasToSchemas } from 'class-validator-jsonschema';
import { getMetadataArgsStorage } from 'routing-controllers';
import { routingControllersToSpec } from 'routing-controllers-openapi';
import swaggerUi from 'swagger-ui-express';

import { App } from '../App';

class Swagger {
  private readonly app: App;

  constructor(app: App) {
    this.app = app;
  }

  init() {
    const schemas = validationMetadatasToSchemas({
      refPointerPrefix: '#/components/schemas/',
    });

    const routingControllersOptions = {
      controllers: this.app.getControllers,
    };

    const storage = getMetadataArgsStorage();

    const spec = routingControllersToSpec(storage, routingControllersOptions, {
      components: {
        schemas,
        securitySchemes: {
          bearerAuth: {
            type: 'http',
            scheme: 'bearer',
            bearerFormat: 'JWT',
          },
        },
      },
      info: {
        description: 'API Generated with `routing-controllers-openapi` package',
        title: 'Free Stuff API',
        version: '1.0.0',
      },
    });

    this.app
      .getHTTPServer()
      .use('/api/swagger', swaggerUi.serve, swaggerUi.setup(spec));
  }
}

export { Swagger };
