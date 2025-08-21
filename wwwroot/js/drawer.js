// Drawer functionality
function toggleDrawer() {
    const sidebar = document.querySelector('.drawer-sidebar');
    const overlay = document.querySelector('.drawer-overlay');
    
    if (sidebar.classList.contains('is-open')) {
        closeDrawer();
    } else {
        openDrawer();
    }
}

function openDrawer() {
    const sidebar = document.querySelector('.drawer-sidebar');
    const overlay = document.querySelector('.drawer-overlay');
    
    sidebar.classList.add('is-open');
    overlay.classList.add('is-active');
    document.body.style.overflow = 'hidden';
}

function closeDrawer() {
    const sidebar = document.querySelector('.drawer-sidebar');
    const overlay = document.querySelector('.drawer-overlay');
    
    sidebar.classList.remove('is-open');
    overlay.classList.remove('is-active');
    document.body.style.overflow = '';
}

// Tab switching functionality
function showTab(tabId, event) {
    // Remove active class from all tab items
    const tabItems = document.querySelectorAll('.tab-item');
    tabItems.forEach(item => {
        item.classList.remove('is-active');
    });
    
    // Add active class to clicked tab item
    if (event && event.currentTarget) {
        const clickedTab = event.currentTarget;
        clickedTab.classList.add('is-active');
    }
    
    // Close the drawer after selecting a tab
    closeDrawer();
    
    // Show content in the main page
    showTabContent(tabId);
}

// Close drawer when clicking outside
document.addEventListener('click', function(event) {
    const sidebar = document.querySelector('.drawer-sidebar');
    const toggle = document.querySelector('.drawer-toggle');
    
    if (sidebar && sidebar.classList.contains('is-open')) {
        if (!sidebar.contains(event.target) && !toggle.contains(event.target)) {
            closeDrawer();
        }
    }
});

// Close drawer with Escape key
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        closeDrawer();
    }
});

// Initialize drawer state
document.addEventListener('DOMContentLoaded', function() {
    // Set initial active tab based on user role
    const isAdmin = document.querySelector('.drawer-sidebar')?.querySelector('.tab-item[onclick*="view-logs"]');
    
    if (isAdmin) {
        // Admin user - start with view-logs tab
        showTabContent('view-logs');
    } else {
        // Regular user - start with my-passwords tab
        showTabContent('my-passwords');
    }
});

// Function to show tab content in the main page
function showTabContent(tabId) {
    const mainContent = document.querySelector('.main-content');
    if (!mainContent) return;
    
    const content = getTabContent(tabId);
    if (content) {
        mainContent.innerHTML = content;
        // Add fade-in animation
        mainContent.style.opacity = '0';
        mainContent.style.transform = 'translateY(20px)';
        setTimeout(() => {
            mainContent.style.transition = 'all 0.3s ease';
            mainContent.style.opacity = '1';
            mainContent.style.transform = 'translateY(0)';
        }, 10);
    }
}

// Function to get tab content
function getTabContent(tabId) {
    const contentMap = {
        'view-logs': `
            <div class="columns is-centered">
                <div class="column is-10">
                    <div class="notification is-info">
                        <h2 class="title is-4">
                            <span class="icon has-text-white">
                                <i class="fas fa-list-alt"></i>
                            </span>
                            Application Logs
                        </h2>
                        <p>Monitor system activities and user actions</p>
                    </div>
                    
                    <div class="card">
                        <div class="card-header">
                            <p class="card-header-title">Recent System Logs</p>
                        </div>
                        <div class="card-content">
                            <div class="content">
                                <table class="table is-fullwidth">
                                    <thead>
                                        <tr>
                                            <th>Timestamp</th>
                                            <th>User</th>
                                            <th>Action</th>
                                            <th>Details</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>2024-01-15 10:30:00</td>
                                            <td>admin</td>
                                            <td>User Login</td>
                                            <td>Successful login from 192.168.1.100</td>
                                        </tr>
                                        <tr>
                                            <td>2024-01-15 10:25:00</td>
                                            <td>ACE-001</td>
                                            <td>Password Reset</td>
                                            <td>Password changed successfully</td>
                                        </tr>
                                        <tr>
                                            <td>2024-01-15 10:20:00</td>
                                            <td>admin</td>
                                            <td>Access Granted</td>
                                            <td>User ACE-002 granted system access</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        'grant-access': `
            <div class="columns is-centered">
                <div class="column is-10">
                    <div class="notification is-warning">
                        <h2 class="title is-4">
                            <span class="icon has-text-white">
                                <i class="fas fa-key"></i>
                            </span>
                            Grant Usage Access
                        </h2>
                        <p>Manage user permissions and access to the system</p>
                    </div>
                    
                    <div class="card">
                        <div class="card-header">
                            <p class="card-header-title">User Access Management</p>
                        </div>
                        <div class="card-content">
                            <div class="field">
                                <label class="label">User ID</label>
                                <div class="control">
                                    <input class="input" type="text" placeholder="Enter User ID (e.g., ACE-XXX)">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Access Level</label>
                                <div class="control">
                                    <div class="select">
                                        <select>
                                            <option>Full Access</option>
                                            <option>Read Only</option>
                                            <option>Limited Access</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="field">
                                <div class="control">
                                    <button class="button is-success">Grant Access</button>
                                    <button class="button is-danger">Revoke Access</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        'reset-password': `
            <div class="columns is-centered">
                <div class="column is-10">
                    <div class="notification is-info">
                        <h2 class="title is-4">
                            <span class="icon has-text-white">
                                <i class="fas fa-lock"></i>
                            </span>
                            Reset Password
                        </h2>
                        <p>Change your password or reset other users' passwords</p>
                    </div>
                    
                    <div class="card">
                        <div class="card-header">
                            <p class="card-header-title">Password Reset Form</p>
                        </div>
                        <div class="card-content">
                            <div class="field">
                                <label class="label">Current Password</label>
                                <div class="control">
                                    <input class="input" type="password" placeholder="Enter current password">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">New Password</label>
                                <div class="control">
                                    <input class="input" type="password" placeholder="Enter new password">
                                </div>
                            </div>
                            <div class="field">
                                <label class="label">Confirm New Password</label>
                                <div class="control">
                                    <input class="input" type="password" placeholder="Confirm new password">
                                </div>
                            </div>
                            <div class="field">
                                <div class="control">
                                    <button class="button is-primary">Update Password</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        'my-passwords': `
            <div class="columns is-centered">
                <div class="column is-10">
                    <div class="notification is-success">
                        <h2 class="title is-4">
                            <span class="icon has-text-white">
                                <i class="fas fa-shield-alt"></i>
                            </span>
                            My Passwords
                        </h2>
                        <p>Manage your personal passwords and credentials securely</p>
                    </div>
                    
                    <div class="card">
                        <div class="card-header">
                            <p class="card-header-title">My Password Vault</p>
                            <div class="card-header-icon">
                                <button class="button is-small is-primary">
                                    <span class="icon">
                                        <i class="fas fa-plus"></i>
                                    </span>
                                    <span>Add New</span>
                                </button>
                            </div>
                        </div>
                        <div class="card-content">
                            <div class="content">
                                <table class="table is-fullwidth">
                                    <thead>
                                        <tr>
                                            <th>Website/Service</th>
                                            <th>Username</th>
                                            <th>Password</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Gmail</td>
                                            <td>user@example.com</td>
                                            <td>••••••••</td>
                                            <td>
                                                <button class="button is-small is-info">View</button>
                                                <button class="button is-small is-warning">Edit</button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>GitHub</td>
                                            <td>username</td>
                                            <td>••••••••</td>
                                            <td>
                                                <button class="button is-small is-info">View</button>
                                                <button class="button is-small is-warning">Edit</button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        'shared-passwords': `
            <div class="columns is-centered">
                <div class="column is-10">
                    <div class="notification is-info">
                        <h2 class="title is-4">
                            <span class="icon has-text-white">
                                <i class="fas fa-share-alt"></i>
                            </span>
                            Shared Passwords
                        </h2>
                        <p>Access passwords shared with you by other users</p>
                    </div>
                    
                    <div class="card">
                        <div class="card-header">
                            <p class="card-header-title">Shared Password Access</p>
                        </div>
                        <div class="card-content">
                            <div class="content">
                                <table class="table is-fullwidth">
                                    <thead>
                                        <tr>
                                            <th>Shared By</th>
                                            <th>Website/Service</th>
                                            <th>Username</th>
                                            <th>Access Level</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>admin</td>
                                            <td>Company Portal</td>
                                            <td>user@company.com</td>
                                            <td>Read Only</td>
                                            <td>
                                                <button class="button is-small is-info">View</button>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>ACE-001</td>
                                            <td>Project Management</td>
                                            <td>project_user</td>
                                            <td>Full Access</td>
                                            <td>
                                                <button class="button is-small is-info">View</button>
                                                <button class="button is-small is-warning">Edit</button>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `
    };
    
    return contentMap[tabId] || '';
}
