from flask import Blueprint, jsonify
from .db import get_organization_activities


bp = Blueprint('activities', __name__)


@bp.route('/organizations/<organization_id>/activities', methods=['GET'])
def index(organization_id):
    """Returns activities for the organization with the
    specified organization id."""

    # return 400 if organization_id is not an int larger than 0
    try:
        id = int(organization_id)
        if id < 1:
            raise ValueError
    except ValueError:
        message = ('Value for organization id must be an '
                   'integer larger than 0. Got \'{}\'.'
                   .format(organization_id))
        response = dict(error=dict(code=400, message=message))

        return jsonify(response), 400

    activities = get_organization_activities(organization_id)

    # return 404 if no activities exist for the provided organization
    if len(activities) == 0:
        message = ('No activities exist for organization '
                   'with id: {}').format(organization_id)
        response = dict(error=dict(code=404, message=message))

        return jsonify(response), 404

    return jsonify(dict(data=activities))
