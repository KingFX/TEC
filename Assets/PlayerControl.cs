using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    private int speed = 10;
    private int rotateSpeed = 10;

    private float yRotation;
    private float xRotation;
    private float currentYRotation;
    private float currentXRotation;
    private float lookSmoothDamp = 0.04f;

    private int lookSensitivity = 8;

    private static Quaternion q;
    private static Vector3 v3;
    private static bool hasHit = false;

    //private void Start() {
    //    //this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, 270, this.transform.rotation.z);
    //}

    // Update is called once per frame
    private Vector3 LastHitPos = new Vector3(100, 100, 100);
    void Update () {

        //if (LastHitPos != Vector3.zero && Vector3.Distance(transform.position, LastHitPos) > 0.25f) {
        if(!hasHit/* && Vector3.Distance(transform.position, LastHitPos) > 0.025f*/) {
            if (Input.GetKey(KeyCode.W)) {
                print("Forwad");
                this.transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S)) {
                print("Backward");
                this.transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.forward, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A)) {
                print("Left");
                this.transform.position = Vector3.MoveTowards(transform.position, transform.position + -transform.right, speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D)) {
                print("Right");
                this.transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.right, speed * Time.deltaTime);
            }
        }
        //Rotation
        //
        yRotation += lookSensitivity * Input.GetAxis("Mouse X");

        xRotation -= lookSensitivity * Input.GetAxis("Mouse Y");
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref currentXRotation, lookSmoothDamp);
        currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref currentYRotation, lookSmoothDamp);

        transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, 0);

        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, currentYRotation, this.transform.rotation.z);
        Camera.main.transform.rotation = Quaternion.Euler(currentXRotation, currentYRotation, this.transform.rotation.z);
        //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.50f, this.transform.position.z);

    }

    private Vector3 v3Old;
    private int frameDelayValue = 3;
    private int frameDelay = 0;
    private List<Vector3> frames = new List<Vector3>();

    void FixedUpdate() {
        //frameDelay++;
        if (hasHit) {
            print("Has Hit");
            hasHit = false;
            //this.transform.position = v3Old;
            this.transform.position = frames[2];
            float distance = 0;
            foreach (Vector3 pos in frames) {
                print("Pos: " + pos);
                if (Vector3.Distance(pos, this.transform.position) > distance) {
                    distance = Vector3.Distance(pos, this.transform.position);
                    this.transform.position = pos;
                }
            }
            GetComponent<Rigidbody>().isKinematic = false;
        } else {
            if (frames.Count > 8) {
                frames.Remove(frames[0]);
                //v3Old = v3;
                //frameDelay = 0;
            }
            LastHitPos = new Vector3(100, 100, 100);
            v3 = this.transform.position;
            bool doesContain = false;
            foreach (Vector3 pos in frames) {
                if (pos == v3) {
                    doesContain = true;
                    break;
                }
            }
            if (!doesContain) {
                frames.Add(v3);
            }
            print("Frames Length: " + frames.Count);
            q = this.transform.rotation;
            //print("V3: " + v3);
        }
    }

    //void OnCollisionStay(Collision col) {
    //    if (col.collider.tag.Equals("Wall")) {
    //        print("Hit:");
    //        //Vector3 v4 = new Vector3(v3.x, 0, 0);
    //        this.GetComponent<Rigidbody>().isKinematic = true;
    //        print("Collider Vect: " + v3);
    //        print("Velocity: " + this.GetComponent<Rigidbody>().velocity);
    //        this.transform.position = v3;
    //        this.transform.rotation = q;
    //        GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        this.GetComponent<Rigidbody>().isKinematic = false;
    //        //GetComponent<Rigidbody>().isKinematic = true;
    //        LastHitPos = v3;
    //        hasHit = true;
    //    }
    //    //foreach (ContactPoint contact in collisionInfo.contacts) {
    //    //    Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    //}
    //}

    void OnCollisionEnter(Collision col) {
        if (col.collider.tag.Equals("Wall")) {
            LastHitPos = v3;
            print("Hit:");
            //Vector3 v4 = new Vector3(v3.x, 0, 0);
            //v3 += -transform.forward / 200;
            this.GetComponent<Rigidbody>().isKinematic = true;
            print("Collider Vect: " + v3);
            print("Velocity: " + this.GetComponent<Rigidbody>().velocity);
            //this.transform.position = v3Old;
            //this.transform.position = v3;
            //this.transform.rotation = q;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            //this.GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<Rigidbody>().isKinematic = true;
            //        LastHitPos = v3;
            hasHit = true;
        }
    }

    //void OnTriggerEnter(Collider col) {
    //    if (col.tag.Equals("Wall")) {
    //        print("Hit:");
    //        //GetComponent<Rigidbody>().isKinematic = true;
    //        hasHit = true;
    //    }
    //}

    //void LateUpdate() {
    //    //print("V3: " + v3);
    //    //hasHit = true;
    //    if (hasHit) {
    //        //print("HasHit");
    //        //transform.position = v3;
    //        //transform.rotation = q;
    //        //GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        hasHit = false;
    //        //GetComponent<Rigidbody>().isKinematic = false;
    //    } else {
    //        v3 = transform.position;
    //        q = transform.rotation;
    //    }
    //}
}
